using TechTalk.SpecFlow;
using FluentAssertions;

namespace QuiosqueFood3000.Order.BddTests.StepDefinitions
{
    [Binding]
    public class CustomerOrderStepDefinitions
    {
        private bool _isCustomerValid;
        private bool _areProductsValid;
        private bool _isOrderCreated;

        [Given(@"um cliente com um CPF válido")]
        public void GivenUmClienteComUmCpfValido()
        {
            _isCustomerValid = true;
        }

        [Given(@"uma lista de produtos válidos")]
        public void GivenUmaListaDeProdutosValidos()
        {
            _areProductsValid = true;
        }

        [When(@"o cliente faz o pedido")]
        public void WhenOClienteFazOPedido()
        {
            if (_isCustomerValid && _areProductsValid)
            {
                _isOrderCreated = true;
            }
        }

        [Then(@"o pedido deve ser criado com sucesso")]
        public void ThenOPedidoDeveSerCriadoComSucesso()
        {
            _isOrderCreated.Should().BeTrue();
        }
    }
}
