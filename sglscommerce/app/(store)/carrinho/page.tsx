"use client";

import { useCart } from "@/features/cart/hooks/use-cart";
import { Button } from "@/components/ui/button";
import Link from "next/link";
import Image from "next/image";
import { Trash2, ShoppingBag } from "lucide-react";

export default function CartPage() {
  const { cart, removeItem, setQuantity, clearCart, totalItems, subtotalCents } = useCart();
  const items = cart?.items || [];
  const subtotal = subtotalCents / 100;

  if (items.length === 0) {
    return (
      <div className="container mx-auto px-4 py-16 text-center">
        <ShoppingBag className="mx-auto h-16 w-16 text-muted-foreground mb-4" />
        <h1 className="text-2xl font-bold mb-2">Seu carrinho está vazio</h1>
        <p className="text-muted-foreground mb-6">Explore nossa loja de roupas e adicione peças incríveis!</p>
        <Button>
          <Link href="/products">Ver Produtos</Link>
        </Button>
      </div>
    );
  }

  return (
    <div className="container mx-auto px-4 py-8">
      <h1 className="text-3xl font-bold mb-8">Carrinho de Compras</h1>
      
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Lista de Itens */}
        <div className="lg:col-span-2 space-y-4">
          {items.map((item: any) => (
            <div key={`${item.id}-${item.size}-${item.color}`} className="flex gap-4 p-4 border rounded-lg bg-card items-center">
              <div className="relative w-20 h-20 bg-muted rounded-md overflow-hidden flex-shrink-0">
                {item.image && (
                  <Image src={item.image} alt={item.name} fill className="object-cover" />
                )}
              </div>
              
              <div className="flex-1">
                <h3 className="font-semibold">{item.name}</h3>
                <p className="text-sm text-muted-foreground">
                  {item.size && `Tamanho: ${item.size}`} {item.color && `| Cor: ${item.color}`}
                </p>
                <span className="font-bold text-primary mt-1 inline-block">
                  R$ {item.price.toFixed(2)}
                </span>
              </div>

              {/* Controle de Quantidade */}
              <div className="flex items-center gap-2">
                <Button 
                  variant="outline" 
                  onClick={() => setQuantity(item.id, item.quantity - 1)}
                >
                  -
                </Button>
                <span className="w-8 text-center">{item.quantity}</span>
                <Button 
                  variant="outline" 
                  onClick={() => setQuantity(item.id, item.quantity + 1)}
                >
                  +
                </Button>
              </div>

              {/* Botão Remover */}
              <Button 
                variant="ghost" 
                className="text-destructive hover:text-destructive/80"
                onClick={() => removeItem(item.id)}
              >
                <Trash2 className="h-5 w-5" />
              </Button>
            </div>
          ))}

          <div className="flex justify-between pt-4">
            <Button variant="outline" onClick={clearCart}>Limpar Carrinho</Button>
            <Button variant="secondary">
              <Link href="/products">Continuar Comprando</Link>
            </Button>
          </div>
        </div>

        {/* Resumo do Pedido */}
        <div className="p-6 border rounded-lg bg-card h-fit space-y-4">
          <h2 className="text-xl font-bold border-b pb-3">Resumo do Pedido</h2>
          <div className="flex justify-between text-sm">
            <span>Itens ({totalItems})</span>
            <span>R$ {subtotal.toFixed(2)}</span>
          </div>
          <div className="flex justify-between text-sm">
            <span>Frete</span>
            <span className="text-green-600 font-medium">Calculado no checkout</span>
          </div>
          <div className="border-t pt-3 flex justify-between font-bold text-lg">
            <span>Total</span>
            <span>R$ {subtotal.toFixed(2)}</span>
          </div>

          <Button className="w-full mt-4">
            <Link href="/checkout" className="w-full block text-center">Finalizar Compra</Link>
          </Button>
        </div>
      </div>
    </div>
  );
}