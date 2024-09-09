using Microsoft.AspNetCore.Mvc;
using MotorcycleRentalManagement.API.Models.Request;
using MotorcycleRentalManagement.API.Models.Requests;
using MotorcycleRentalManagement.Domain.Interfaces.Usercases;
namespace MotorcycleRentalManagement.API.Controllers
{
    [ApiController]
    [Route("api/entregadores")]
    public class DeliveryPersonController : ControllerBase
    {
        private readonly IRegisterDeliveryPersonUseCase _registerDeliveryPersonUseCase;
        private readonly IUpdateDeliveryPersonCnhImageUseCase _updateCnhImageUseCase;

        public DeliveryPersonController(
            IRegisterDeliveryPersonUseCase registerDeliveryPersonUseCase,
            IUpdateDeliveryPersonCnhImageUseCase updateCnhImageUseCase)
        {
            _registerDeliveryPersonUseCase = registerDeliveryPersonUseCase;
            _updateCnhImageUseCase = updateCnhImageUseCase;
        }


        /// <summary>
        /// Cadastrar um entregador.
        /// </summary>
        /// <remarks>
        /// Esta rota permite que novos entregadores sejam cadastrados na plataforma.
        /// </remarks>
        /// <param name="request">Dados do entregador.</param>
        /// <response code="201">Entregador cadastrado com sucesso.</response>
        /// <response code="400">Dados inválidos na requisição.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(201)] 
        [ProducesResponseType(typeof(DefaultResponse), 400)]  
        public async Task<IActionResult> Register([FromBody] RegisterDeliveryPersonRequest request)
        {
            var result = await _registerDeliveryPersonUseCase.ExecuteAsync(
                request.Identificador,
                request.Nome,
                request.Cnpj,
                request.DataNascimento,
                request.NumeroCnh,
                request.TipoCnh,
                request.ImagemCnh);

            return result.IsValid ? Created("", new { Identificador = request.Identificador }) : BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
        }

        /// <summary>
        /// Atualizar a foto da CNH de um entregador.
        /// </summary>
        /// <remarks>
        /// Esta rota permite que o entregador envie uma nova foto da CNH para atualizar seu cadastro.
        /// </remarks>
        /// <param name="id">O identificador do entregador.</param>
        /// <param name="request">Imagem da CNH codificada em Base64.</param>
        /// <response code="200">Foto da CNH atualizada com sucesso.</response>
        /// <response code="400">Erro na atualização da foto da CNH.</response>
        [HttpPost("{id}/cnh")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(DefaultResponse), 400)]
        public async Task<IActionResult> UpdateCnhImage([FromRoute] string id, [FromBody] UpdateCnhImageRequest request)
        {
            var result = await _updateCnhImageUseCase.ExecuteAsync(id, request.ImagemCnh);

            return result.IsValid ? Created("", new { Identificador = id }) : BadRequest(new DefaultResponse() { Mensagem = string.Join("\n", result.Notifications.Select(n => n.Message)) });
        }
    }
}