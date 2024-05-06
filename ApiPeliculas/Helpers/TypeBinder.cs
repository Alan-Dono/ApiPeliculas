using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace ApiPeliculas.Helpers
{
    public class TypeBinder<T> : IModelBinder // lo hacemos generico
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var nombrePropiedad = bindingContext.ModelName;
            var proveedorValores = bindingContext.ValueProvider.GetValue(nombrePropiedad);

            if (proveedorValores == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            try
            {
                var valorDeserealizado = JsonConvert.DeserializeObject<T>(proveedorValores.FirstValue); // Version generica
                //var valorDeserealizado = JsonConvert.DeserializeObject<List<int>>(proveedorValores.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserealizado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, "Vlor invalido para tipo List<int>");
            }

            return Task.CompletedTask;
        }
    }
}
