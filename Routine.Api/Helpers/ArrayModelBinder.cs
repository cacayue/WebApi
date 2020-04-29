using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Routine.Api.Helpers
{
    public class ArrayModelBinder:IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            //判断传入的model是否为数组类型
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            //判断model能否获取值
            if (string.IsNullOrWhiteSpace(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            //反射获取model的类型,IEnumerable<CompanyAddDto> 传入的model只有一种类型,因而取第一个
            var elementType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];
            var converter = TypeDescriptor.GetConverter(elementType);
            //将单个字符串分割为数组,并且转换为object类型
            var values = value.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=>converter.ConvertFromString(x.Trim())).ToArray();
            //创建空的array实例,类型为elementType
            var typedValues = Array.CreateInstance(elementType, values.Length);
            //将空的Array赋值,由0开始
            values.CopyTo(typedValues,0);
            bindingContext.Model = typedValues;

            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}