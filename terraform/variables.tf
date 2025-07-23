variable "aws_region" {
  description = "Região da AWS para criar os recursos"
  type        = string
  default     = "us-east-1"
}

variable "eks_cluster_name" {
  description = "Nome do cluster EKS"
  type        = string
  default     = "quiosque-food-cluster"
}

variable "ecr_repository_name" {
  description = "Nome do repositório ECR"
  type        = string
  default     = "quiosque-food-repository"
}
