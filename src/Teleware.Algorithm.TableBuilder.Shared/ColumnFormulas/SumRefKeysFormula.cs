using System;
using System.Collections.Generic;
using System.Linq;
using Teleware.Algorithm.TableBuilder.Rows;

namespace Teleware.Algorithm.TableBuilder.ColumnFormulas
{
    /// <summary>
    /// 根据键名计算总和公式
    /// </summary>
    public class SumRefKeysFormula<TValue> : IFormula
        where TValue : struct
    {
        private static readonly Func<dynamic, TValue?> _id = v => (TValue?)v;

        /// <summary>
        /// 初始化公式
        /// </summary>
        /// <param name="valueMapper">值映射器，将列数据转为对应的值</param>
        /// <param name="adder">加法器</param>
        /// <param name="refKeys">键名列表</param>
        public SumRefKeysFormula(Func<dynamic, TValue?> valueMapper, Func<TValue?, TValue?, TValue?> adder, params string[] refKeys)
        {
            RefKeys = refKeys;
            ValueMapper = valueMapper;
            Adder = adder;
        }

        /// <summary>
        /// 初始化公式
        /// </summary>
        /// <param name="adder">加法器</param>
        /// <param name="refKeys">键名列表</param>
        public SumRefKeysFormula(Func<TValue?, TValue?, TValue?> adder, params string[] refKeys)
        {
            RefKeys = refKeys;
            ValueMapper = _id;
            Adder = adder;
        }

        /// <summary>
        /// 加法器
        /// </summary>
        public Func<TValue?, TValue?, TValue?> Adder { get; }

        /// <summary>
        /// 键名列表
        /// </summary>
        public string[] RefKeys { get; }

        /// <summary>
        /// 值映射器，将列数据转为对应的值
        /// </summary>
        public Func<dynamic, TValue?> ValueMapper { get; }

        /// <see cref="IFormula.Execute(IEnumerable{DataRow})"/>
        public dynamic Execute(IEnumerable<DataRow> rows)
        {
            var values = rows.SelectMany(row => RefKeys.Select(k =>
            {
                var columnData = row.RowBuildContext.GetColumnDataByRefKey(k);
                if (columnData != null)
                {
                    return ValueMapper(columnData);
                }
                return null;
            })).Where(v => v != null);
            return values.Aggregate((TValue?)null, (acc, v) => Adder((acc.HasValue ? acc.Value : default(TValue)), v));
        }
    }

    /// <summary>
    /// 根据键名计算总和公式
    /// </summary>
    /// <remarks>
    /// <see cref="decimal"/>优化版
    /// </remarks>
    public class SumRefKeysDecimalFormula : IFormula
    {
        private static Func<dynamic, decimal?> _id = v => (decimal?)v;

        /// <summary>
        /// 初始化公式
        /// </summary>
        /// <param name="valueGetter">取值器，获取列数据对应的值</param>
        /// <param name="refKeys">键名列表</param>
        public SumRefKeysDecimalFormula(Func<dynamic, decimal?> valueGetter, params string[] refKeys)
        {
            RefKeys = refKeys;
            ValueGetter = valueGetter;
        }

        /// <summary>
        /// 初始化公式
        /// </summary>
        /// <param name="refKeys">键名列表</param>
        public SumRefKeysDecimalFormula(params string[] refKeys)
        {
            RefKeys = refKeys;
            ValueGetter = _id;
        }

        /// <summary>
        /// 加法计算器
        /// </summary>

        /// <summary>
        /// 键名列表
        /// </summary>
        public string[] RefKeys { get; }

        /// <summary>
        /// 取值器，获取列数据对应的值
        /// </summary>
        public Func<dynamic, decimal?> ValueGetter { get; }

        /// <see cref="IFormula.Execute(IEnumerable{DataRow})"/>
        public dynamic Execute(IEnumerable<DataRow> rows)
        {
            decimal? sum = null;

            foreach (var row in rows)
            {
                foreach (var refKey in RefKeys)
                {
                    var columnData = row.RowBuildContext.GetColumnDataByRefKey(refKey);
                    if (columnData == null)
                    {
                        continue;
                    }
                    var value = ValueGetter(columnData);
                    if (value != null)
                    {
                        sum = (sum ?? 0M) + value;
                    }
                }
            }

            return sum;
        }
    }
}