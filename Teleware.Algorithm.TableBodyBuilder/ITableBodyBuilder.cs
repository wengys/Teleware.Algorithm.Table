using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teleware.Algorithm.TableBuilder.Shared;

namespace Teleware.Algorithm.TableBodyBuilder
{
    public interface ITableBodyBuilder
    {
        TableBody Build(dynamic rawDatas);
    }
}