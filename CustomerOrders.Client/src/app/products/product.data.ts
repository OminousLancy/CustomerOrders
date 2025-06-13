export interface ProductDto {
  id?: number;
  name: string;
  price: number;
}

export interface ProductCreateDto {
  name: string;
  price: number;
}
