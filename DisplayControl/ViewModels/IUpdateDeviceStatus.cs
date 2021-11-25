using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using DisplayControl.Models;

namespace DisplayControl.ViewModels
{
    interface IUpdateDeviceStatus
    {
        void UpdateCellColor((DeviceStatusColumn Col, DeviceStatusRow Row) targetDeviceStatusCell, SolidColorBrush colorBrush);
    }
}
