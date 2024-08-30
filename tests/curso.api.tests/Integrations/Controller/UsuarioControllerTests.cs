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
using System.Threading.Tasks;
using AutoBogus;

namespace curso.api.tests.Integrations.Controller
{
    public class UsuarioControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {

        private readonly WebApplicationFactory<Startup> _factory;
        private readonly HttpClient _httpClient;
        private readonly ITestOutputHelper _output;
        protected RegistroViewModelInput registroViewModelInput;

        public UsuarioControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _httpClient = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }

        public async Task InitializeAsync()
        {
            await Registrar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso();
        }

        //WhenGivenThen
        [Fact]
        public async Task Logar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = registroViewModelInput.Login,
                Senha = registroViewModelInput.Senha
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput), Encoding.UTF8, "application/json");

            // Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/logar", content);

            var loginViewModelOutput = JsonConvert.DeserializeObject<LoginViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(loginViewModelOutput.Token);
            Assert.Equal(loginViewModelInput.Login, loginViewModelOutput.Usuario.Login);
            _output.WriteLine(loginViewModelOutput.Token);
        }

        [Fact]
        public async Task Registrar_InformandoUsuarioESenhaExistentes_DeveRetornarSucesso()
        {
            // Arrange
            registroViewModelInput = new AutoFaker<RegistroViewModelInput>()
                                                .RuleFor(p => p.Email, faker => faker.Person.Email);

            StringContent content = new StringContent(JsonConvert.SerializeObject(registroViewModelInput), Encoding.UTF8, "application/json");

            // Act
            var httpClientRequest = await _httpClient.PostAsync("api/v1/usuario/registrar", content);

            // Assert
            _output.WriteLine(httpClientRequest.Content.ReadAsStringAsync().Result);
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

    }
}
