terraform {
  backend "s3" {
    bucket = "quiosque-food-order"
    key    = "terraform.tfstate"
    region = "us-east-1"
  }
}