using System.Collections.Generic;

// A simple stack implementation to get around the fact that using
// System.Collections.Generic.Stack<T> pulls in an extra DLL on the webplayer.
public class LiteStack<T> {
  private int idx = 0;
  private readonly List<T> stack = new List<T>();

  public void Push(T obj) {
    idx++;
    if(idx > stack.Count)
      stack.Add(obj);
    else
      stack[idx - 1] = obj;
  }

  public T Pop() {
    T tmp = Peek();
    if(idx > 0) {
      idx--;
      stack[idx] = default(T);
    }
    return tmp;
  }

  public T Peek() {
    if(idx > 0)
      return stack[idx - 1];
    return default(T);
  }

  public int Count {
    get { return idx; }
  }

  public void Clear() {
    stack.Clear();
    idx = 0;
  }
}
