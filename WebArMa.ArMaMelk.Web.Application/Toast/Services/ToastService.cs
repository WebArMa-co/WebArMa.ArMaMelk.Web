using WebArMa.ArMaMelk.Web.Application.Toast.Dtos;
using WebArMa.ArMaMelk.Web.Application.Toast.Enums;

namespace WebArMa.ArMaMelk.Web.Application.Toast.Services
{
	public class ToastService
	{
		public event Action? OnChanged;

		private List<ToastMessage> _toasts = new();

		public IReadOnlyList<ToastMessage> Toasts => _toasts;

		public void ShowSuccess(string message) => ShowToast(message, ToastLevel.Success);

		public void ShowError(string message) => ShowToast(message, ToastLevel.Error);

		public void ShowWarning(string message) => ShowToast(message, ToastLevel.Warning);

		public void ShowInfo(string message) => ShowToast(message, ToastLevel.Info);

		private async void ShowToast(string message, ToastLevel level)
		{
			var toast = new ToastMessage { Message = message, Level = level };
			_toasts.Add(toast);
			OnChanged?.Invoke();

			// حذف خودکار پیام پس از 3.5 ثانیه
			await Task.Delay(3500);

			_toasts.Remove(toast);
			OnChanged?.Invoke();
		}

		public void RemoveToast(ToastMessage toast)
		{
			_toasts.Remove(toast);
			OnChanged?.Invoke();
		}
	}
}