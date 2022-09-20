using System;
using AltenTest.Core.DomainObjects;

namespace AltenTest.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}