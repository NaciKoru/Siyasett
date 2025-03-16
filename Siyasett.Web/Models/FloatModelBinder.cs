

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Siyasett.Web.Models;


public class FloatModelBinder : IModelBinder
{


    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        //var valueProviderResult = bindingContext
        //    .ValueProvider
        //    .GetValue(bindingContext.ModelName);

        //var cultureInfo = new CultureInfo("tr");
        //cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
        //cultureInfo.NumberFormat.NumberGroupSeparator = ",";
        //float model=0;
        //var value = valueProviderResult.FirstValue;
        //if(value!=null && !string.IsNullOrEmpty(value))
        //{ 
        //    if(float.TryParse(valueProviderResult.FirstValue.Replace(",", "."),NumberStyles.Number, cultureInfo, out model))
        //    {
        //        bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
        //        bindingContext.Result = ModelBindingResult.Success(model);
        //    }
        //}




        //return Task.CompletedTask;


        if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));

        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult != ValueProviderResult.None)
        {
            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var valueAsString = valueProviderResult.FirstValue;
            float result;

            // Use invariant culture
            if (float.TryParse(valueAsString.Replace(",", "."), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
                return Task.CompletedTask;
            }
        }

        // If we haven't handled it, then we'll let the base SimpleTypeModelBinder handle it
        return Task.CompletedTask;


    }
}


public class FloatModelBinderProvider : IModelBinderProvider
{
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context.Metadata.ModelType == typeof(float))
            return new FloatModelBinder();

        return null;
    }
}
