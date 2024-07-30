using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared.Properties;
using System.Windows;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for StartWizardPart.xaml
    /// </summary>
    public partial class StartWizardPart : AB_WizardPartBase
    {
        public const string Step_Start = "Start";

        public StartWizardPart() : this(null)
        {

        }

        public StartWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();            
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Start, DescriptionResource.WIZARDSTART, DescriptionResource.ORDERENTRYWIZARD, "FlagGreen_small.png"));
        }

        public override void am_SetWizardButtonStates()
        {
            ap_BtnNextVisibility = Visibility.Visible;
            ap_BtnBackVisibility = Visibility.Collapsed;
            ap_BtnResetVisibility = Visibility.Collapsed;
            ap_BtnSkipVisibility = Visibility.Collapsed;
            ap_BtnFinishVisibility = Visibility.Collapsed;
        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            am_MarkCurrentStepComplete();
        }
    }
}
