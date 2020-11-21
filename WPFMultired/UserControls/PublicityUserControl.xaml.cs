using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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
                    _imageSleader = new ImageSleader(null, Utilities.GetConfiguration("PathPublish"));

                    this.DataContext = _imageSleader.imageModel;

                    _imageSleader.time = 3;

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
