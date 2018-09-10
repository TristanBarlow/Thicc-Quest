using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ISave
{
    void LoadSuccess(object obj);
    void LoadFailed();
}