using AppMvcFull.Business.Models;
using FluentValidation;

namespace AppMvcFull.Business.Validations
{
    public class SupplierValidation : AbstractValidator<Supplier>
    {
        public SupplierValidation()
        {
            RuleFor(f => f.Name)
                 .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                 .Length(2, 100)
                 .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            When(f => f.SupplierType == SupplierType.Natural, () =>
            {
                RuleFor(f => f.DocumentNumber.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CpfValidacao.Validar(f.DocumentNumber)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });

            When(f => f.SupplierType == SupplierType.Legal, () =>
            {
                RuleFor(f => f.DocumentNumber.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo Documento precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");
                RuleFor(f => CnpjValidacao.Validar(f.DocumentNumber)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }    
    }
}
