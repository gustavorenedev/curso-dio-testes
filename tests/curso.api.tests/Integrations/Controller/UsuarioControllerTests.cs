using Bogus.Extensions;
using curso.api.Models.Usuarios;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Net.Http;
using Xunit;
using Xunit.Abstractions;
using Microsoft.VisualStudio.TestPlatform.Utilities;

namespace curso.api.tests.Integrations.Controller
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
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

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput), Encoding.UTF8, "application/json");

            // Act
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/logar", content).GetAwaiter().GetResult();

            var loginViewModelOutput = JsonConvert.DeserializeObject<LoginViewModelOutput>(httpClientRequest.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(loginViewModelOutput.Token);
            Assert.Equal(loginViewModelInput.Login, loginViewModelOutput.Usuario.Login);
            _output.WriteLine(loginViewModelOutput.Token);
        }

        [Fact]
        public void Registrar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var registroViewModelInput = new RegistroViewModelInput
            {
                Login = "gustavorene",
                Email = "gustavo@gmail.com",
                Senha = "123456"
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(registroViewModelInput), Encoding.UTF8, "application/json");

            // Act
            var httpClientRequest = _httpClient.PostAsync("api/v1/usuario/registrar", content).GetAwaiter().GetResult();

            // Assert
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

    }
}
