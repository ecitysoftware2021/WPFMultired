using System;
using System.Reflection;
using System.Windows.Controls;
using WPFMultired.Classes;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Interaction logic for PublicityUserControl.xaml
    /// </summary>
    public partial class PublicityUserControl : UserControl
    {
        #region "Referencias"
        private ImageSleader _imageSleader;
        #endregion

        #region "Constructor"
        public PublicityUserControl()
        {
            InitializeComponent();
            ConfiguratePublish();
        }
        #endregion

        #region "Métodos"
        private void ConfiguratePublish()
        {
            try
            {
                if (_imageSleader == null)
                {
                    _imageSleader = new ImageSleader(null, Utilities.GetConfiguration("PathPublish2"));

                    this.DataContext = _imageSleader.imageModel;

                    _imageSleader.time = 5;

                    _imageSleader.isRotate = true;

                    _imageSleader.Start();

                    this.DataContext = _imageSleader.imageModel;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }
        #endregion
    }
}
