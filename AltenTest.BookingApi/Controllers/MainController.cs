using AltenTest.Core.Communication;
using AltenTest.Core.Messages;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AltenTest.BookingApi.Controllers
{
    [ApiController]
    public abstract class MainController : ControllerBase
    {
        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
            {
                return Ok(result);
            }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Mensagens", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);
            foreach (var erro in erros)
            {
                AddProcessingError(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var erro in validationResult.Errors)
            {
                AddProcessingError(erro.ErrorMessage);
            }

            return CustomResponse();
        }

        protected ActionResult CustomResponse(CommandResponse commandResponse)
        {
            if (!commandResponse.IsSucess)
            {
                foreach (var erro in commandResponse.ValidationResult.Errors)
                {
                    AddProcessingError(erro.ErrorMessage);
                }

                return CustomResponse();
            }

            return Ok(commandResponse);

        }

        protected ActionResult CustomResponse(ResponseResult resposta)
        {
            ContainsResponseErrors(resposta);

            return CustomResponse();
        }

        protected bool ContainsResponseErrors(ResponseResult resposta)
        {
            if (resposta == null || !resposta.Errors.Mensagens.Any()) return false;

            foreach (var mensagem in resposta.Errors.Mensagens)
            {
                AddProcessingError(mensagem);
            }

            return true;
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddProcessingError(string erro)
        {
            Errors.Add(erro);
        }

        protected void ClearProcessingErrors()
        {
            Errors.Clear();
        }
    }
}