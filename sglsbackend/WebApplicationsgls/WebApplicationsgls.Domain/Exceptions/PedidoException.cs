using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationsgls.Domain.Exceptions;

public class PedidoException : DomainException
{
    public PedidoException(string message)
        : base(message)
    {
    }
}
