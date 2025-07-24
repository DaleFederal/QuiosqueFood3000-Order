provider "aws" {
  region = var.aws_region
}

# Bucket S3 para guardar artefatos de deploy
resource "aws_s3_bucket" "deploy_artifacts" {
  bucket = "quiosque-deploy-artifacts-${random_id.bucket_suffix.hex}"
}

resource "random_id" "bucket_suffix" {
  byte_length = 8
}

# Role para a instância EC2 ter acesso ao ECR, SSM e S3
resource "aws_iam_role" "app_role" {
  name = "ec2-app-role"
  assume_role_policy = jsonencode({
    Version = "2012-10-17",
    Statement = [
      {
        Action = "sts:AssumeRole",
        Effect = "Allow",
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      }
    ]
  })
}

# Anexa as políticas necessárias à Role
resource "aws_iam_role_policy_attachment" "ssm_policy" {
  role       = aws_iam_role.app_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonSSMManagedInstanceCore"
}

resource "aws_iam_role_policy_attachment" "ecr_policy" {
  role       = aws_iam_role.app_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly"
}

resource "aws_iam_role_policy_attachment" "s3_policy" {
  role       = aws_iam_role.app_role.name
  policy_arn = "arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess" # Acesso de leitura ao S3
}


# Repositório para a imagem Docker da API
resource "aws_ecr_repository" "api_repo" {
  name = var.ecr_repo_name
}

# Security Group para a instância EC2
resource "aws_security_group" "app_sg" {
  name        = "app-sg"
  description = "Allow app traffic"

  ingress {
    from_port   = 5000 # Porta da sua API
    to_port     = 5000
    protocol    = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

# Script de inicialização da instância EC2
# Instala Docker, Docker Compose e inicia os contêineres
locals {
  user_data = <<-EOT
              #!/bin/bash
              sudo yum update -y
              sudo yum install -y docker aws-cli
              sudo service docker start
              sudo usermod -a -G docker ec2-user

              # Instalar Docker Compose
              sudo curl -L https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m) -o /usr/local/bin/docker-compose
              sudo chmod +x /usr/local/bin/docker-compose

              # Criar diretório da aplicação
              mkdir -p /home/ec2-user/app
              cd /home/ec2-user/app

              # Salvar a senha do banco em um arquivo de ambiente
              echo "POSTGRES_PASSWORD=${var.db_password}" > .env
              
              # Baixar o docker-compose.prod.yml
              cat <<'EOF' > docker-compose.prod.yml
services:
  app:
    image: ${aws_ecr_repository.api_repo.repository_url}:latest
    ports:
      - "5000:8080"
    depends_on:
      - db
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=QuiosqueFood3000;Username=postgres;Password=${POSTGRES_PASSWORD}

  db:
    image: postgres:latest
    container_name: postgres_container
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=${POSTGRES_PASSWORD}
      - POSTGRES_DB=QuiosqueFood3000
    volumes:
      - db-data:/var/lib/postgresql/data
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql

volumes:
  db-data:
EOF
              EOT
}

resource "aws_iam_instance_profile" "app_profile" {
  name = "app-instance-profile"
  role = aws_iam_role.app_role.name
}

# Instância EC2
resource "aws_instance" "app_server" {
  ami           = "ami-0c55b159cbfafe1f0" # Amazon Linux 2 AMI (us-east-1)
  instance_type = "t2.micro"
  iam_instance_profile = aws_iam_instance_profile.app_profile.name
  security_groups = [aws_security_group.app_sg.name]
  user_data = local.user_data

  tags = {
    Name = "AppServer-Quiosque"
  }
}
