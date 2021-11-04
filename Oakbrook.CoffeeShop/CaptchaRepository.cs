using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class CaptchaRepository : Repository, ICaptchaRepository
  {
    public CaptchaRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public Captcha Get(int id)
    {
      return Query($"SELECT Id, Question, Answer FROM Captcha WHERE Id = {id}", reader =>
      {
        if (reader.Read())
        {
          return new Captcha
          {
            Id = id,
            Answer = reader.GetString(2),
            Question = reader.GetString(1),
          };
        }

        return null;
      });
    }

    public List<Captcha> GetAll()
    {
      return Query($"SELECT Id, Question, Answer FROM Captcha", reader =>
      {
        List<Captcha> captchas = new List<Captcha>();
        while (reader.Read())
        {
          captchas.Add(new Captcha
          {
            Id = reader.GetInt32(0),
            Answer = reader.GetString(2),
            Question = reader.GetString(1),
          });
        }

        return captchas;
      });
    }
  }
}