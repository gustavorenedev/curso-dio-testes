using Bogus.Extensions;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Xunit;

namespace curso.api.tests.Integrations.Controller
{
    public class UsuarioControllerTests
    {

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        //WhenGivenThen
        [Fact]
        public void Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = "gustavorene",
                Senha = "123456"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput));

            // Act
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/logar", content).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
        }


    }
}
