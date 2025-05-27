using System;

namespace BloggingPlatfromAPI.Data;

public interface IUnitOfWork
{
        Task<int> CompleteAsync();
}
