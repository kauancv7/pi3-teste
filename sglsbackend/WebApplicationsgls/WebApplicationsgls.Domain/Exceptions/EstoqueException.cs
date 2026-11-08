using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationsgls.Domain.Exceptions;

public class EstoqueException : DomainException
{
    public EstoqueException(string message)
        : base(message)
    {
    }
}