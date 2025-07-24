output "ecr_repository_url" {
  description = "URL do repositório ECR"
  value       = aws_ecr_repository.app_ecr_repo.repository_url
}

output "ec2_public_ip" {
  description = "IP público da instância EC2"
  value       = aws_instance.app_server.public_ip
}