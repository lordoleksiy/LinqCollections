using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionsAndLinq.BL.BLData;

public class Data
{
    public static Dictionary<int, string> TaskStateMapper { get; } = new()
    {
        { 0, "To Do" },
        { 1, "In Progress"},
        { 2, "Done"},
        { 3, "Canceled"}
    };
}
