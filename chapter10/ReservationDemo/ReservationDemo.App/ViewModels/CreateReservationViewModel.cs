using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;
using DotVVM.Framework.Hosting;
using System.Threading;
using ReservationDemo.Domain.Exceptions;
using ReservationDemo.Domain.Model;
using ReservationDemo.Domain.Services;

namespace ReservationDemo.App.ViewModels
{
    public class CreateReservationViewModel(ReservationService reservationService) : DotvvmViewModelBase
    {
        public CreateReservationModel Model { get; set; } = new()
        {
            SelectedRoomId = 1,
            SelectedDay = new DateOnly(2024, 1, 1)
        };

        [Bind(Direction.ServerToClient)]
        public string ErrorMessage { get; set; }

        public async Task CreateReservation()
        {
            CreateReservationResult result;
            try
            {
                result = await reservationService.CreateReservation(Model);
            }
            catch (DomainException ex)
            {
                ErrorMessage = ex.Message;
                return;
            }
            Context.RedirectToRoute("ReservationSummary", result);
        }
    }
}

