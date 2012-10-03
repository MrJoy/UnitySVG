using System.Collections.Generic;

// A simple stack implementation to get around the fact that using
// System.Collections.Generic.Stack<T> pulls in an extra DLL on the webplayer.
public class LiteStack<T> {
  private List<T> stack = new List<T>();

  public void Push(T obj) {
    stack.Add(obj);
  }

  public T Pop() {
    T tmp = Peek();
    if(stack.Count > 0)
      stack.RemoveAt(stack.Count - 1);
    return tmp;
  }

  public T Peek() {
    if(stack.Count > 0)
      return stack[stack.Count - 1];
    else
      return default(T);
  }

  public int Count {
    get {
      return stack.Count;
    }
  }

  public void Clear() {
    stack.Clear();
  }
}

public class LiteStack : LiteStack<object> {

}
