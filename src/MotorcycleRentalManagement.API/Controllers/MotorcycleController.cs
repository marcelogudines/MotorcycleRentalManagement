using Microsoft.AspNetCore.Mvc;
using MotorcycleRentalManagement.API.Models.Request;
using MotorcycleRentalManagement.API.Models.Requests;
using MotorcycleRentalManagement.Domain.Interfaces.Repositories;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.API.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotorcycleController : ControllerBase
    {
        private readonly IRegisterMotorcycleMotorcycleUseCase _registermotorcycleUseCase;
        private readonly IUpdateMotorcycleLicensePlateUseCase _updateMotorcycleLicensePlateUseCase;
        private readonly IMotorcycleRepository _motorcycleRepository;
        private readonly IMotorcycleUseCase _motorcycleUsecase;
        private readonly IDeleteMotorcycleUseCase _deleteMotorcycleUseCase;
        public MotorcycleController(IRegisterMotorcycleMotorcycleUseCase registermotorcycleUseCase,
            IMotorcycleUseCase motorcycleUseCase, IMotorcycleRepository motorcycleRepository, 
            IUpdateMotorcycleLicensePlateUseCase updateMotorcycleLicensePlateUseCase, IDeleteMotorcycleUseCase deleteMotorcycleUseCase)
        {
            _motorcycleUsecase = motorcycleUseCase;
            _motorcycleRepository = motorcycleRepository;
            _registermotorcycleUseCase = registermotorcycleUseCase;
            _updateMotorcycleLicensePlateUseCase = updateMotorcycleLicensePlateUseCase;
            _deleteMotorcycleUseCase = deleteMotorcycleUseCase;
        }


        /// <summary>
        /// Cadastra uma nova moto no sistema.
        /// </summary>
        /// <remarks>
        /// Esta rota é utilizada para registrar uma nova moto no sistema, exigindo que o usuário tenha permissões de administrador.
        /// </remarks>
        /// <param name="request">Objeto contendo os dados da moto: Id, Ano, Modelo, e Placa.</param>
        /// <response code="201">Moto cadastrada com sucesso.</response>
        /// <response code="400">Erro na validação dos dados.</response>
        [HttpPost("")]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(201)] 
        [ProducesResponseType(typeof(DefaultResponse), 400)] 
        public async Task<IActionResult> RegisterMotorcycle([FromBody] RegisterMotorcycleRequest request)
        {
            var result = await _registermotorcycleUseCase.RegisterMotorcycleAsync(request.Identificador, request.Ano, request.Modelo, request.Placa);
            return result.IsValid ? Created("", new { Identificador = request.Identificador }) : BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
        }


        /// <summary>
        /// Consulta motos cadastradas filtrando pela placa.
        /// </summary>
        /// <remarks>
        /// Esta rota permite que o usuário admin consulte motos cadastradas na plataforma.
        /// O campo da placa é opcional, mas se informado, trará os registros correspondentes.
        /// </remarks>
        /// <param name="placa">A placa da moto a ser consultada. Exemplo: "ABC-1234".</param>
        /// <response code="200">Retorna uma lista de motos correspondentes ao filtro.</response>
        [HttpGet]
        //[Authorize(Roles = "Admin")]  // Apenas Admins podem consultar motos
        [ProducesResponseType(typeof(List<MotorcycleResponse>), 200)] 
        public async Task<IActionResult> GetMotorcycles([FromQuery] string? placa)
        {
            var result = await _motorcycleRepository.GetMotorcyclesAsync(placa);

            var response = result.Select(m => new MotorcycleResponse
            {
                Identificador = m.Id,
                Ano = m.Year,
                Modelo = m.Model,
                Placa = m.LicensePlate
            }).ToList();

            return Ok(response);
        }

        /// <summary>
        /// Modifica a placa de uma moto existente.
        /// </summary>
        /// <param name="id">O identificador da moto.</param>
        /// <param name="request">Dados da placa da moto.</param>
        /// <response code="200">Placa modificada com sucesso.</response>
        /// <response code="400">Erro nos dados de entrada.</response>
        [HttpPut("{id}/placa")]
        [ProducesResponseType(typeof(DefaultResponse), 200)] // Retorna status 200 com mensagem
        [ProducesResponseType(typeof(DefaultResponse), 400)]  // Define o código de erro para BadRequest
        public async Task<IActionResult> UpdateMotorcycleLicensePlate([FromRoute] string id, [FromBody] UpdateLicensePlateRequest request)
        {
            var result = await _updateMotorcycleLicensePlateUseCase.ExecuteAsync(id, request.Placa);

            if (!result.IsValid)
            {
                return  BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
            }

            return Ok(new DefaultResponse() { Mensagem = "Placa modificada com sucesso" });
        }

        // <summary>
        /// Remove uma moto do sistema.
        /// </summary>
        /// <param name="id">O identificador da moto.</param>
        /// <response code="200">Moto removida com sucesso.</response>
        /// <response code="400">Erro na requisição.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DefaultResponse), 200)] // Retorna status 200 com mensagem
        [ProducesResponseType(typeof(DefaultResponse), 400)]  // Define o código de erro para BadRequest
        public async Task<IActionResult> DeleteMotorcycle([FromRoute] string id)
        {
            var result = await _deleteMotorcycleUseCase.ExecuteAsync(id);

            if (!result.IsValid)
            {
                return BadRequest(new DefaultResponse { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
            }

            return Ok(new DefaultResponse { Mensagem = "Moto removida com sucesso" });
        }
    }   
}
