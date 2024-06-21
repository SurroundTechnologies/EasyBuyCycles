using A4DN.Core.BOS.FrameworkEntity;
using A4DN.Core.WPF.Base.WizardBase;
using BOS.OrderManagement.Shared.Properties;

namespace WPF.Wizards.OrderWizard
{
    /// <summary>
    /// Interaction logic for StartWizardPart.xaml
    /// </summary>
    public partial class StartWizardPart : AB_WizardPartBase
    {
        public const string Step_Start = "WPF.Wizards.OrderWizard.StartWizardPart";

        public StartWizardPart() : this(null)
        {

        }

        public StartWizardPart(AB_WizardPartInitArgs InputArgs) : base(InputArgs)
        {
            InitializeComponent();
        }

        protected override void am_EnrollWizardSteps()
        {
            am_AddStep(new AB_WizardStep(Step_Start, DescriptionResource.WIZARDSTART, DescriptionResource.ORDERWIZARDSPLASH, "MyCompanySquare_48_48_32.png"));
        }

        public override void am_InitializeOrResetWizardPart()
        {

        }

        public override void am_OnNextClick(AB_WizardButtonClickEventArgs Args)
        {
            am_MarkCurrentStepComplete();
        }
    }
}
