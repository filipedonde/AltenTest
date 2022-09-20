using System.Threading.Tasks;
using AltenTest.Core.Messages;
using FluentValidation.Results;

namespace AltenTest.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evento) where T : Event;
        Task<CommandResponse> SendCommand<T>(T comando) where T : Command;
    }
}