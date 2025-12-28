using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechLogistics.Application.Interfaces
{
    public interface IMessageProducer
    {
        // Enviaremos cualquier objeto (T) a una cola específica
        void SendMessage<T>(T message);
    }
}
