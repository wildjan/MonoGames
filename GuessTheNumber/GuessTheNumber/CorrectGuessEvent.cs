using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessTheNumber
{
    /// <summary>
    /// Delegate for handling the event
    /// </summary>
    /// <param name="numberTile"></param>
    public delegate void CorrectGuessEventHandler(NumberTile numberTile);

    /// <summary>
    /// An event that indicates that the correct number tile was guessed
    /// </summary>
    public class CorrectGuessEvent
    {
        // the event handlers registered to receive the event
        event CorrectGuessEventHandler eventHandlers;

        /// <summary>
        /// Register the given event handler
        /// </summary>
        /// <param name="handler">the event handler</param>
        public void Register(CorrectGuessEventHandler handler)
        {
            eventHandlers += handler;
        }

        /// <summary>
        /// Trigger the event for all event handlers
        /// </summary>
        /// <param name="numberTile"></param>
        public void OnCorrectGuessEvent(NumberTile numberTile)
        {
            if (eventHandlers != null)
            {
                eventHandlers(numberTile);
            }
        }
    }
}
