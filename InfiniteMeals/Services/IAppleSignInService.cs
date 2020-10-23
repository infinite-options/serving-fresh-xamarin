using System;
using System.Threading.Tasks;
using InfiniteMeals.Models;

namespace InfiniteMeals.Services
{
    public interface IAppleSignInService
    {
        bool IsAvailable { get; }
        Task<AppleSignInCredentialState> GetCredentialStateAsync(string userId);
        Task<AppleAccount> SignInAsync();
    }
}
