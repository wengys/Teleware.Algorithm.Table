using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared;

namespace Teleware.Algorithm.TableBodyBuilder
{
    public interface IRowDataPicker
    {
        IEnumerable<IDictionary<string, dynamic>> PickRowDatas(dynamic rawDatas);
    }
}