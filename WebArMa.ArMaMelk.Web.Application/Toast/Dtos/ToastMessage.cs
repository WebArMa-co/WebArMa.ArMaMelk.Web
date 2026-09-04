using WebArMa.ArMaMelk.Web.Application.Toast.Enums;

namespace WebArMa.ArMaMelk.Web.Application.Toast.Dtos
{
    public class ToastMessage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public ToastLevel Level { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
