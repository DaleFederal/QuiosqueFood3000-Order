# variables.tf - Atualizado para AWS Academy
variable "aws_region" {
  description = "Região da AWS para criar os recursos"
  type        = string
  default     = "us-east-1"
}

variable "instance_type" {
  description = "Tipo da instância EC2"
  type        = string
  default     = "t2.micro"  # Free tier
  
  validation {
    condition = contains([
      "t2.micro", "t2.small", "t2.medium",
      "t3.micro", "t3.small", "t3.medium"
    ], var.instance_type)
    error_message = "Tipo de instância deve ser um dos tipos permitidos."
  }
}

variable "ami_id" {
  description = "ID da AMI para a instância EC2 (Ubuntu 22.04 LTS)"
  type        = string
  default     = "ami-0e86e20dae90224e1"  # Ubuntu Server 22.04 LTS (HVM), SSD Volume Type - us-east-1
}

variable "key_pair_name" {
  description = "Nome do par de chaves SSH existente na AWS"
  type        = string
  
  validation {
    condition     = length(var.key_pair_name) > 0
    error_message = "O nome do par de chaves não pode estar vazio."
  }
}

variable "private_key_path" {
  description = "Caminho para o arquivo da chave privada SSH"
  type        = string
  default     = "~/.ssh/id_rsa"
}

variable "github_repository" {
  description = "URL do repositório GitHub (opcional para deploy automático)"
  type        = string
  default     = ""
}

variable "project_name" {
  description = "Nome do projeto (usado nas tags)"
  type        = string
  default     = "QuiosqueFood3000"
}

variable "environment" {
  description = "Ambiente de deployment"
  type        = string
  default     = "production"
  
  validation {
    condition = contains([
      "development", "staging", "production"
    ], var.environment)
    error_message = "Ambiente deve ser: development, staging ou production."
  }
}