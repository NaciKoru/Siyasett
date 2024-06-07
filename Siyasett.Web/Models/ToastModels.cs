using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siyasett.Models
{
    [Serializable]

    public enum ToastType
    {
        error,
        info,
        success,
        warning,
        question
    }

    [Serializable]
    public class Toastr
    {
        public bool ShowNewestOnTop { get; set; }
        public bool ShowCloseButton { get; set; }
        public List<ToastMessage> ToastMessages { get; set; }


        public ToastMessage AddToastMessage(string title, string message, ToastType toastType)
        {

            var toast = new ToastMessage()
            {
                Title = title,
                Message = message,
                ToastType = toastType
            };
            ToastMessages.Add(toast);
            return toast;
        }
        public Toastr()
        {
            ToastMessages = new List<ToastMessage>();
            ShowCloseButton = false;
            ShowNewestOnTop = false;
        }

    }

    [Serializable]
    public class ToastMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public ToastType ToastType { get; set; }
        public bool IsSticky { get; set; }

    }

}
