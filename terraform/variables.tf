variable "aws_region" {
  description = "Região da AWS para criar os recursos"
  type        = string
  default     = "us-east-1"
}

variable "ecr_repository_name" {
  description = "Nome do repositório ECR"
  type        = string
  default     = "quiosque-food-repository"
}

variable "instance_type" {
  description = "Tipo da instância EC2"
  type        = string
  default     = "t2.micro"
}

variable "ami_id" {
  description = "ID da AMI para a instância EC2 (Ubuntu 22.04 LTS)"
  type        = string
  default     = "ami-053b0d53c279acc90" # Exemplo: Ubuntu Server 22.04 LTS (HVM), SSD Volume Type
}

variable "key_pair_name" {
  description = "Nome do par de chaves SSH existente na AWS"
  type        = string
}