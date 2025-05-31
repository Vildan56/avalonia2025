using ReactiveUI;

namespace Project2025.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        public RealEstateViewModel RealEstateVM { get; } = new();
        public ClientViewModel ClientVM { get; } = new();
        public RealtorViewModel RealtorVM { get; } = new();
    }
} 