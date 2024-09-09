using Microsoft.AspNetCore.Mvc;
using MotorcycleRentalManagement.API.Models.Request;
using MotorcycleRentalManagement.API.Models.Requests;
using MotorcycleRentalManagement.Domain.Interfaces;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;

namespace MotorcycleRentalManagement.API.Controllers
{
    [ApiController]
    [Route("api/locacoes")]
    public class RentalController : ControllerBase
    {
        private readonly IRegisterRentalUseCase _rentalUseCase;
        private readonly IReturnRentalUseCase _returnRentalUseCase;
        private readonly IGetRentalByIdUseCase _getRentalByIdUseCase;

        public RentalController(
            IRegisterRentalUseCase rentalUseCase,
            IReturnRentalUseCase returnRentalUseCase,
            IGetRentalByIdUseCase getRentalByIdUseCase)
        {
            _rentalUseCase = rentalUseCase;
            _returnRentalUseCase = returnRentalUseCase;
            _getRentalByIdUseCase = getRentalByIdUseCase;
        }

        /// <summary>
        /// Criar uma locação para um entregador.
        /// </summary>
        /// <remarks>
        /// Esta rota permite que um entregador crie uma locação de moto.
        /// </remarks>
        /// <param name="request">Dados da locação.</param>
        /// <response code="201">Locação criada com sucesso.</response>
        /// <response code="400">Dados inválidos na requisição.</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(DefaultResponse), 400)]
        public async Task<IActionResult> CreateRental([FromBody] RegisterRentalRequest request)
        {
            var (result, id) = await _rentalUseCase.ExecuteAsync(
                request.EntregadorId,
                request.MotoId,
                request.DataTermino,
                request.DataPrevisaoTermino);

            return result.IsValid ? Created("", new { Identificador = id }) :
                BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
        }

        /// <summary>
        /// Informar a data de devolução e calcular o valor final da locação.
        /// </summary>
        /// <remarks>
        /// Esta rota permite que o entregador informe a data de devolução da moto e o sistema calcule o valor total da locação, incluindo multas ou acréscimos, se aplicável.
        /// </remarks>
        /// <param name="id">Identificador da locação.</param>
        /// <param name="request">Dados de devolução.</param>
        /// <response code="200">Valor final calculado com sucesso.</response>
        /// <response code="400">Erro nos dados de entrada ou locação não encontrada.</response>
        [HttpPut("{id}/devolucao")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(DefaultResponse), 400)]
        public async Task<IActionResult> ReturnRental([FromRoute] Guid id, [FromBody] ReturnRentalRequest request)
        {
            var result = await _returnRentalUseCase.ExecuteAsync(id, request.DataDevolucao);

            return result.IsValid ? Ok(new DefaultResponse() { Mensagem = "Data de devolução informada com sucesso" }) :
                BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
        }

        /// <summary>
        /// Consultar locação por ID.
        /// </summary>
        /// <remarks>
        /// Esta rota permite consultar os detalhes de uma locação por ID.
        /// </remarks>
        /// <param name="id">Identificador da locação.</param>
        /// <response code="200">Locação consultada com sucesso.</response>
        /// <response code="404">Locação não encontrada.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRentalById([FromRoute] Guid id)
        {
            var rental = await _getRentalByIdUseCase.ExecuteAsync(id);

            if (rental == null)
            {
                return NotFound(new DefaultResponse() { Mensagem = "Locação não encontrada" });
            }

            var response = new RentalResponse
            {
                Identificador = rental.Id.ToString(),
                ValorDiaria = rental.PricePerDay,
                EntregadorId = rental.DeliveryPersonId,
                MotoId = rental.MotorcycleId,
                DataInicio = rental.StartDate,
                DataTermino = rental.EndDate,
                DataPrevisaoTermino = rental.ExpectedEndDate,
                DataDevolucao = rental.ReturnDate
            };

            return Ok(response);
        }
    }
}
