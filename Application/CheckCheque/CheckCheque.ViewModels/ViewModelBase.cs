using System;
using Xamarin.Forms;

namespace CheckCheque.ViewModels
{
    public abstract class ViewModelBase: BindableObject
    {
        public INavigation Navigation { get; }

        protected ViewModelBase(INavigation navigation)
        {
            Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }
    }
}
