using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ultimate.WebCore.Models.Catalog;

namespace Ultimate.WebCore.Validation
{
    public class CourseUpdateInputValidator:AbstractValidator<CourseUpdateInput>
    {
        public CourseUpdateInputValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("isim alanı boş geçilemez");
            RuleFor(x => x.Description).NotEmpty().WithMessage("açıklama alanı boş geçilemez");
            RuleFor(x => x.Feature.Duration).InclusiveBetween(1, int.MaxValue).WithMessage("süre alanı boş geçilemez");

            //$$$$,$$ scale virgülden sonra kaç karakter oalcağaını precision ise toplam kaç karakter oalcağını beliritr.

            RuleFor(x => x.Price).NotEmpty().WithMessage("açıklama alanı boş geçilemez").ScalePrecision(2, 6).WithMessage("hatalı para formatı");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("kategori alanı seçiniz");
        }
    }
}
