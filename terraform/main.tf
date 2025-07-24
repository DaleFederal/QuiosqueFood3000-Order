provider "aws" {
  region = var.aws_region
}

data "aws_iam_role" "lab_role" {
  name = "LabRole"
}

resource "aws_iam_instance_profile" "lab_instance_profile" {
  name = "LabRole-instance-profile" 
  role = data.aws_iam_role.lab_role.name
}

# Bucket S3 para guardar artefatos de deploy
resource "aws_s3_bucket" "deploy_artifacts" {
  bucket = "quiosque-deploy-artifacts-${random_id.bucket_suffix.hex}"
}

resource "random_id" "bucket_suffix" {
  byte_length = 8
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
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=QuiosqueFood3000;Username=postgres;Password=$${POSTGRES_PASSWORD}

  db:
    image: postgres:latest
    container_name: postgres_container
    environment:
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=$${POSTGRES_PASSWORD}
      - POSTGRES_DB=QuiosqueFood3000
    volumes:
      - db-data:/var/lib/postgresql/data
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql

volumes:
  db-data:
EOF
              EOT
}

# Instância EC2
resource "aws_instance" "app_server" {
  ami           = "ami-0cbbe2c6a1bb2ad63" # AMI do Amazon Linux 2 para us-east-1 (verifique se é a correta para você)
  instance_type = "t2.micro"
  # Atribui o Instance Profile pré-existente do AWS Academy
  iam_instance_profile = aws_iam_instance_profile.lab_instance_profile.name
  security_groups = [aws_security_group.app_sg.name]
  user_data = local.user_data

  tags = {
    Name = "AppServer-Quiosque"
  }
}