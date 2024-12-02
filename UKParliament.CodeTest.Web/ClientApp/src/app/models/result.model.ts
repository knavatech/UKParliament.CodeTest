export interface Result<T> {
  Data: T;
  Message: string;
  StatusCode: number;
  Success: boolean;
}
