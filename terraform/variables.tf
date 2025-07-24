variable "aws_region" {
  description = "Região da AWS para criar os recursos"
  type        = string
  default     = "us-east-1"
}

variable "ecr_repo_name" {
  description = "Nome do repositório ECR para a API"
  type        = string
  default     = "quiosquefood3000-api"
}

variable "db_password" {
  description = "Senha para o banco de dados PostgreSQL"
  type        = string
  sensitive   = true # Marca a variável como sensível
}
