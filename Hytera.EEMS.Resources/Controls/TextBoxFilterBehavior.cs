using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Hytera.EEMS.Resources.Controls
{
    public class TextBoxFilterBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.KeyDown += new KeyEventHandler(AssociatedObject_KeyDown);
            CommandBinding com = new CommandBinding();
            com.Command = ApplicationCommands.Paste;
            com.CanExecute += com_CanExecute;
            this.AssociatedObject.CommandBindings.Add(com);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.KeyDown -= new KeyEventHandler(AssociatedObject_KeyDown);
        }

        void com_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            int code = (int)e.Key;
            if (!(code == 3 || code == 6 ||
                (code >= 44 && code <= 69) ||
                (code == 143 && (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) ||
                (code >= 74 && code <= 83) ||
                (e.KeyboardDevice.Modifiers & ModifierKeys.Shift) != ModifierKeys.Shift && code >= 34 && code <= 43))
            {
                e.Handled = true;
            }
        }
    }
}
