# BoundedQueue

Bounded Queue
Class Kata “Bounded Queue”
Entwickle eine Warteschlangenklasse mit begrenzter Länge für die Kommunikation zwischen mehreren Threads.

Lesende Threads entnehmen Elemente; falls die Queue leer ist, blockieren sie und warten auf das nächste Element.

Schreibende Threads fügen Elemente an; falls die Queue voll ist, blockieren sie und warten darauf, dass ein anderer Thread ein Element entnimmt.

Das Interface der Klasse soll so aussehen:
```
class BoundedQueue<T> {
	BoundedQueue(int size) {...}
	void Enqueue(T element) {...}
	T Dequeue() {...}
	int Count() {...} // Anzahl Elemente in Queue
	int Size() {...} // Max. Anzahl Elemente
}
```
Beispiel einer Nutzung:



Vernachlässige Performancegesichtspunkte.

Variation #1
Erweitere die Klasse um zwei Funktionen:
```
class BoundedQueue<T> {
	...
	bool TryEnqueue(T element, int timeoutMsec) {...}
	bool TryDequeue(int timeoutMsec, out T element) {...}
}
```
Das Einstellen/Auslesen soll optional nur für eine gewisse Zeitspanne blockieren. Falls die Aktion in dieser Zeit erfolgreich ist, wird true zurückgeliefert, sonst false.
