output "ecr_repository_url" {
  description = "URL do repositório ECR criado"
  value       = aws_ecr_repository.api_repo.repository_url
}

output "instance_public_ip" {
  description = "IP público da instância EC2"
  value       = aws_instance.app_server.public_ip
}

output "instance_id" {
  description = "ID da instância EC2 para o SSM"
  value       = aws_instance.app_server.id
}

output "s3_bucket_name" {
  description = "Nome do bucket S3 para artefatos"
  value       = aws_s3_bucket.deploy_artifacts.bucket
}
