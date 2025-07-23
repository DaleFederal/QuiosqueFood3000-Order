output "ecr_repository_url" {
  description = "URL do repositório ECR"
  value       = aws_ecr_repository.app_ecr_repo.repository_url
}

output "eks_cluster_name" {
  description = "Nome do cluster EKS"
  value       = module.eks.cluster_name
}

output "eks_cluster_endpoint" {
  description = "Endpoint do cluster EKS"
  value       = module.eks.cluster_endpoint
}

output "eks_cluster_security_group_id" {
  description = "ID do security group do cluster EKS"
  value       = module.eks.cluster_security_group_id
}
