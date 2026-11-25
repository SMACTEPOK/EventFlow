package com.eventflow.inventory.v1;

import static io.grpc.MethodDescriptor.generateFullMethodName;

/**
 * <pre>
 * Internal contract between Booking and Inventory bounded contexts.
 * </pre>
 */
@io.grpc.stub.annotations.GrpcGenerated
public final class InventoryServiceGrpc {

  private InventoryServiceGrpc() {}

  public static final java.lang.String SERVICE_NAME = "eventflow.inventory.v1.InventoryService";

  // Static method descriptors that strictly reflect the proto.
  private static volatile io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CheckAvailabilityRequest,
      com.eventflow.inventory.v1.CheckAvailabilityResponse> getCheckAvailabilityMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "CheckAvailability",
      requestType = com.eventflow.inventory.v1.CheckAvailabilityRequest.class,
      responseType = com.eventflow.inventory.v1.CheckAvailabilityResponse.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CheckAvailabilityRequest,
      com.eventflow.inventory.v1.CheckAvailabilityResponse> getCheckAvailabilityMethod() {
    io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CheckAvailabilityRequest, com.eventflow.inventory.v1.CheckAvailabilityResponse> getCheckAvailabilityMethod;
    if ((getCheckAvailabilityMethod = InventoryServiceGrpc.getCheckAvailabilityMethod) == null) {
      synchronized (InventoryServiceGrpc.class) {
        if ((getCheckAvailabilityMethod = InventoryServiceGrpc.getCheckAvailabilityMethod) == null) {
          InventoryServiceGrpc.getCheckAvailabilityMethod = getCheckAvailabilityMethod =
              io.grpc.MethodDescriptor.<com.eventflow.inventory.v1.CheckAvailabilityRequest, com.eventflow.inventory.v1.CheckAvailabilityResponse>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "CheckAvailability"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.CheckAvailabilityRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.CheckAvailabilityResponse.getDefaultInstance()))
              .setSchemaDescriptor(new InventoryServiceMethodDescriptorSupplier("CheckAvailability"))
              .build();
        }
      }
    }
    return getCheckAvailabilityMethod;
  }

  private static volatile io.grpc.MethodDescriptor<com.eventflow.inventory.v1.ReserveSeatsRequest,
      com.eventflow.inventory.v1.ReserveSeatsResponse> getReserveSeatsMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "ReserveSeats",
      requestType = com.eventflow.inventory.v1.ReserveSeatsRequest.class,
      responseType = com.eventflow.inventory.v1.ReserveSeatsResponse.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<com.eventflow.inventory.v1.ReserveSeatsRequest,
      com.eventflow.inventory.v1.ReserveSeatsResponse> getReserveSeatsMethod() {
    io.grpc.MethodDescriptor<com.eventflow.inventory.v1.ReserveSeatsRequest, com.eventflow.inventory.v1.ReserveSeatsResponse> getReserveSeatsMethod;
    if ((getReserveSeatsMethod = InventoryServiceGrpc.getReserveSeatsMethod) == null) {
      synchronized (InventoryServiceGrpc.class) {
        if ((getReserveSeatsMethod = InventoryServiceGrpc.getReserveSeatsMethod) == null) {
          InventoryServiceGrpc.getReserveSeatsMethod = getReserveSeatsMethod =
              io.grpc.MethodDescriptor.<com.eventflow.inventory.v1.ReserveSeatsRequest, com.eventflow.inventory.v1.ReserveSeatsResponse>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "ReserveSeats"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.ReserveSeatsRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.ReserveSeatsResponse.getDefaultInstance()))
              .setSchemaDescriptor(new InventoryServiceMethodDescriptorSupplier("ReserveSeats"))
              .build();
        }
      }
    }
    return getReserveSeatsMethod;
  }

  private static volatile io.grpc.MethodDescriptor<com.eventflow.inventory.v1.WatchReservationStatusRequest,
      com.eventflow.inventory.v1.ReservationStatusEvent> getWatchReservationStatusMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "WatchReservationStatus",
      requestType = com.eventflow.inventory.v1.WatchReservationStatusRequest.class,
      responseType = com.eventflow.inventory.v1.ReservationStatusEvent.class,
      methodType = io.grpc.MethodDescriptor.MethodType.SERVER_STREAMING)
  public static io.grpc.MethodDescriptor<com.eventflow.inventory.v1.WatchReservationStatusRequest,
      com.eventflow.inventory.v1.ReservationStatusEvent> getWatchReservationStatusMethod() {
    io.grpc.MethodDescriptor<com.eventflow.inventory.v1.WatchReservationStatusRequest, com.eventflow.inventory.v1.ReservationStatusEvent> getWatchReservationStatusMethod;
    if ((getWatchReservationStatusMethod = InventoryServiceGrpc.getWatchReservationStatusMethod) == null) {
      synchronized (InventoryServiceGrpc.class) {
        if ((getWatchReservationStatusMethod = InventoryServiceGrpc.getWatchReservationStatusMethod) == null) {
          InventoryServiceGrpc.getWatchReservationStatusMethod = getWatchReservationStatusMethod =
              io.grpc.MethodDescriptor.<com.eventflow.inventory.v1.WatchReservationStatusRequest, com.eventflow.inventory.v1.ReservationStatusEvent>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.SERVER_STREAMING)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "WatchReservationStatus"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.WatchReservationStatusRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.ReservationStatusEvent.getDefaultInstance()))
              .setSchemaDescriptor(new InventoryServiceMethodDescriptorSupplier("WatchReservationStatus"))
              .build();
        }
      }
    }
    return getWatchReservationStatusMethod;
  }

  private static volatile io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CancelOperationRequest,
      com.eventflow.inventory.v1.CancelOperationResponse> getCancelOperationMethod;

  @io.grpc.stub.annotations.RpcMethod(
      fullMethodName = SERVICE_NAME + '/' + "CancelOperation",
      requestType = com.eventflow.inventory.v1.CancelOperationRequest.class,
      responseType = com.eventflow.inventory.v1.CancelOperationResponse.class,
      methodType = io.grpc.MethodDescriptor.MethodType.UNARY)
  public static io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CancelOperationRequest,
      com.eventflow.inventory.v1.CancelOperationResponse> getCancelOperationMethod() {
    io.grpc.MethodDescriptor<com.eventflow.inventory.v1.CancelOperationRequest, com.eventflow.inventory.v1.CancelOperationResponse> getCancelOperationMethod;
    if ((getCancelOperationMethod = InventoryServiceGrpc.getCancelOperationMethod) == null) {
      synchronized (InventoryServiceGrpc.class) {
        if ((getCancelOperationMethod = InventoryServiceGrpc.getCancelOperationMethod) == null) {
          InventoryServiceGrpc.getCancelOperationMethod = getCancelOperationMethod =
              io.grpc.MethodDescriptor.<com.eventflow.inventory.v1.CancelOperationRequest, com.eventflow.inventory.v1.CancelOperationResponse>newBuilder()
              .setType(io.grpc.MethodDescriptor.MethodType.UNARY)
              .setFullMethodName(generateFullMethodName(SERVICE_NAME, "CancelOperation"))
              .setSampledToLocalTracing(true)
              .setRequestMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.CancelOperationRequest.getDefaultInstance()))
              .setResponseMarshaller(io.grpc.protobuf.ProtoUtils.marshaller(
                  com.eventflow.inventory.v1.CancelOperationResponse.getDefaultInstance()))
              .setSchemaDescriptor(new InventoryServiceMethodDescriptorSupplier("CancelOperation"))
              .build();
        }
      }
    }
    return getCancelOperationMethod;
  }

  /**
   * Creates a new async stub that supports all call types for the service
   */
  public static InventoryServiceStub newStub(io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<InventoryServiceStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<InventoryServiceStub>() {
        @java.lang.Override
        public InventoryServiceStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new InventoryServiceStub(channel, callOptions);
        }
      };
    return InventoryServiceStub.newStub(factory, channel);
  }

  /**
   * Creates a new blocking-style stub that supports all types of calls on the service
   */
  public static InventoryServiceBlockingV2Stub newBlockingV2Stub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<InventoryServiceBlockingV2Stub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<InventoryServiceBlockingV2Stub>() {
        @java.lang.Override
        public InventoryServiceBlockingV2Stub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new InventoryServiceBlockingV2Stub(channel, callOptions);
        }
      };
    return InventoryServiceBlockingV2Stub.newStub(factory, channel);
  }

  /**
   * Creates a new blocking-style stub that supports unary and streaming output calls on the service
   */
  public static InventoryServiceBlockingStub newBlockingStub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<InventoryServiceBlockingStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<InventoryServiceBlockingStub>() {
        @java.lang.Override
        public InventoryServiceBlockingStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new InventoryServiceBlockingStub(channel, callOptions);
        }
      };
    return InventoryServiceBlockingStub.newStub(factory, channel);
  }

  /**
   * Creates a new ListenableFuture-style stub that supports unary calls on the service
   */
  public static InventoryServiceFutureStub newFutureStub(
      io.grpc.Channel channel) {
    io.grpc.stub.AbstractStub.StubFactory<InventoryServiceFutureStub> factory =
      new io.grpc.stub.AbstractStub.StubFactory<InventoryServiceFutureStub>() {
        @java.lang.Override
        public InventoryServiceFutureStub newStub(io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
          return new InventoryServiceFutureStub(channel, callOptions);
        }
      };
    return InventoryServiceFutureStub.newStub(factory, channel);
  }

  /**
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public interface AsyncService {

    /**
     */
    default void checkAvailability(com.eventflow.inventory.v1.CheckAvailabilityRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CheckAvailabilityResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getCheckAvailabilityMethod(), responseObserver);
    }

    /**
     */
    default void reserveSeats(com.eventflow.inventory.v1.ReserveSeatsRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReserveSeatsResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getReserveSeatsMethod(), responseObserver);
    }

    /**
     */
    default void watchReservationStatus(com.eventflow.inventory.v1.WatchReservationStatusRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReservationStatusEvent> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getWatchReservationStatusMethod(), responseObserver);
    }

    /**
     * <pre>
     * Cancels an active inventory reservation operation.
     * </pre>
     */
    default void cancelOperation(com.eventflow.inventory.v1.CancelOperationRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CancelOperationResponse> responseObserver) {
      io.grpc.stub.ServerCalls.asyncUnimplementedUnaryCall(getCancelOperationMethod(), responseObserver);
    }
  }

  /**
   * Base class for the server implementation of the service InventoryService.
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public static abstract class InventoryServiceImplBase
      implements io.grpc.BindableService, AsyncService {

    @java.lang.Override public final io.grpc.ServerServiceDefinition bindService() {
      return InventoryServiceGrpc.bindService(this);
    }
  }

  /**
   * A stub to allow clients to do asynchronous rpc calls to service InventoryService.
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public static final class InventoryServiceStub
      extends io.grpc.stub.AbstractAsyncStub<InventoryServiceStub> {
    private InventoryServiceStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected InventoryServiceStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new InventoryServiceStub(channel, callOptions);
    }

    /**
     */
    public void checkAvailability(com.eventflow.inventory.v1.CheckAvailabilityRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CheckAvailabilityResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getCheckAvailabilityMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void reserveSeats(com.eventflow.inventory.v1.ReserveSeatsRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReserveSeatsResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getReserveSeatsMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     */
    public void watchReservationStatus(com.eventflow.inventory.v1.WatchReservationStatusRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReservationStatusEvent> responseObserver) {
      io.grpc.stub.ClientCalls.asyncServerStreamingCall(
          getChannel().newCall(getWatchReservationStatusMethod(), getCallOptions()), request, responseObserver);
    }

    /**
     * <pre>
     * Cancels an active inventory reservation operation.
     * </pre>
     */
    public void cancelOperation(com.eventflow.inventory.v1.CancelOperationRequest request,
        io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CancelOperationResponse> responseObserver) {
      io.grpc.stub.ClientCalls.asyncUnaryCall(
          getChannel().newCall(getCancelOperationMethod(), getCallOptions()), request, responseObserver);
    }
  }

  /**
   * A stub to allow clients to do synchronous rpc calls to service InventoryService.
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public static final class InventoryServiceBlockingV2Stub
      extends io.grpc.stub.AbstractBlockingStub<InventoryServiceBlockingV2Stub> {
    private InventoryServiceBlockingV2Stub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected InventoryServiceBlockingV2Stub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new InventoryServiceBlockingV2Stub(channel, callOptions);
    }

    /**
     */
    public com.eventflow.inventory.v1.CheckAvailabilityResponse checkAvailability(com.eventflow.inventory.v1.CheckAvailabilityRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getCheckAvailabilityMethod(), getCallOptions(), request);
    }

    /**
     */
    public com.eventflow.inventory.v1.ReserveSeatsResponse reserveSeats(com.eventflow.inventory.v1.ReserveSeatsRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getReserveSeatsMethod(), getCallOptions(), request);
    }

    /**
     */
    @io.grpc.ExperimentalApi("https://github.com/grpc/grpc-java/issues/10918")
    public io.grpc.stub.BlockingClientCall<?, com.eventflow.inventory.v1.ReservationStatusEvent>
        watchReservationStatus(com.eventflow.inventory.v1.WatchReservationStatusRequest request) {
      return io.grpc.stub.ClientCalls.blockingV2ServerStreamingCall(
          getChannel(), getWatchReservationStatusMethod(), getCallOptions(), request);
    }

    /**
     * <pre>
     * Cancels an active inventory reservation operation.
     * </pre>
     */
    public com.eventflow.inventory.v1.CancelOperationResponse cancelOperation(com.eventflow.inventory.v1.CancelOperationRequest request) throws io.grpc.StatusException {
      return io.grpc.stub.ClientCalls.blockingV2UnaryCall(
          getChannel(), getCancelOperationMethod(), getCallOptions(), request);
    }
  }

  /**
   * A stub to allow clients to do limited synchronous rpc calls to service InventoryService.
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public static final class InventoryServiceBlockingStub
      extends io.grpc.stub.AbstractBlockingStub<InventoryServiceBlockingStub> {
    private InventoryServiceBlockingStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected InventoryServiceBlockingStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new InventoryServiceBlockingStub(channel, callOptions);
    }

    /**
     */
    public com.eventflow.inventory.v1.CheckAvailabilityResponse checkAvailability(com.eventflow.inventory.v1.CheckAvailabilityRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getCheckAvailabilityMethod(), getCallOptions(), request);
    }

    /**
     */
    public com.eventflow.inventory.v1.ReserveSeatsResponse reserveSeats(com.eventflow.inventory.v1.ReserveSeatsRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getReserveSeatsMethod(), getCallOptions(), request);
    }

    /**
     */
    public java.util.Iterator<com.eventflow.inventory.v1.ReservationStatusEvent> watchReservationStatus(
        com.eventflow.inventory.v1.WatchReservationStatusRequest request) {
      return io.grpc.stub.ClientCalls.blockingServerStreamingCall(
          getChannel(), getWatchReservationStatusMethod(), getCallOptions(), request);
    }

    /**
     * <pre>
     * Cancels an active inventory reservation operation.
     * </pre>
     */
    public com.eventflow.inventory.v1.CancelOperationResponse cancelOperation(com.eventflow.inventory.v1.CancelOperationRequest request) {
      return io.grpc.stub.ClientCalls.blockingUnaryCall(
          getChannel(), getCancelOperationMethod(), getCallOptions(), request);
    }
  }

  /**
   * A stub to allow clients to do ListenableFuture-style rpc calls to service InventoryService.
   * <pre>
   * Internal contract between Booking and Inventory bounded contexts.
   * </pre>
   */
  public static final class InventoryServiceFutureStub
      extends io.grpc.stub.AbstractFutureStub<InventoryServiceFutureStub> {
    private InventoryServiceFutureStub(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      super(channel, callOptions);
    }

    @java.lang.Override
    protected InventoryServiceFutureStub build(
        io.grpc.Channel channel, io.grpc.CallOptions callOptions) {
      return new InventoryServiceFutureStub(channel, callOptions);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<com.eventflow.inventory.v1.CheckAvailabilityResponse> checkAvailability(
        com.eventflow.inventory.v1.CheckAvailabilityRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getCheckAvailabilityMethod(), getCallOptions()), request);
    }

    /**
     */
    public com.google.common.util.concurrent.ListenableFuture<com.eventflow.inventory.v1.ReserveSeatsResponse> reserveSeats(
        com.eventflow.inventory.v1.ReserveSeatsRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getReserveSeatsMethod(), getCallOptions()), request);
    }

    /**
     * <pre>
     * Cancels an active inventory reservation operation.
     * </pre>
     */
    public com.google.common.util.concurrent.ListenableFuture<com.eventflow.inventory.v1.CancelOperationResponse> cancelOperation(
        com.eventflow.inventory.v1.CancelOperationRequest request) {
      return io.grpc.stub.ClientCalls.futureUnaryCall(
          getChannel().newCall(getCancelOperationMethod(), getCallOptions()), request);
    }
  }

  private static final int METHODID_CHECK_AVAILABILITY = 0;
  private static final int METHODID_RESERVE_SEATS = 1;
  private static final int METHODID_WATCH_RESERVATION_STATUS = 2;
  private static final int METHODID_CANCEL_OPERATION = 3;

  private static final class MethodHandlers<Req, Resp> implements
      io.grpc.stub.ServerCalls.UnaryMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.ServerStreamingMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.ClientStreamingMethod<Req, Resp>,
      io.grpc.stub.ServerCalls.BidiStreamingMethod<Req, Resp> {
    private final AsyncService serviceImpl;
    private final int methodId;

    MethodHandlers(AsyncService serviceImpl, int methodId) {
      this.serviceImpl = serviceImpl;
      this.methodId = methodId;
    }

    @java.lang.Override
    @java.lang.SuppressWarnings("unchecked")
    public void invoke(Req request, io.grpc.stub.StreamObserver<Resp> responseObserver) {
      switch (methodId) {
        case METHODID_CHECK_AVAILABILITY:
          serviceImpl.checkAvailability((com.eventflow.inventory.v1.CheckAvailabilityRequest) request,
              (io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CheckAvailabilityResponse>) responseObserver);
          break;
        case METHODID_RESERVE_SEATS:
          serviceImpl.reserveSeats((com.eventflow.inventory.v1.ReserveSeatsRequest) request,
              (io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReserveSeatsResponse>) responseObserver);
          break;
        case METHODID_WATCH_RESERVATION_STATUS:
          serviceImpl.watchReservationStatus((com.eventflow.inventory.v1.WatchReservationStatusRequest) request,
              (io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.ReservationStatusEvent>) responseObserver);
          break;
        case METHODID_CANCEL_OPERATION:
          serviceImpl.cancelOperation((com.eventflow.inventory.v1.CancelOperationRequest) request,
              (io.grpc.stub.StreamObserver<com.eventflow.inventory.v1.CancelOperationResponse>) responseObserver);
          break;
        default:
          throw new AssertionError();
      }
    }

    @java.lang.Override
    @java.lang.SuppressWarnings("unchecked")
    public io.grpc.stub.StreamObserver<Req> invoke(
        io.grpc.stub.StreamObserver<Resp> responseObserver) {
      switch (methodId) {
        default:
          throw new AssertionError();
      }
    }
  }

  public static final io.grpc.ServerServiceDefinition bindService(AsyncService service) {
    return io.grpc.ServerServiceDefinition.builder(getServiceDescriptor())
        .addMethod(
          getCheckAvailabilityMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              com.eventflow.inventory.v1.CheckAvailabilityRequest,
              com.eventflow.inventory.v1.CheckAvailabilityResponse>(
                service, METHODID_CHECK_AVAILABILITY)))
        .addMethod(
          getReserveSeatsMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              com.eventflow.inventory.v1.ReserveSeatsRequest,
              com.eventflow.inventory.v1.ReserveSeatsResponse>(
                service, METHODID_RESERVE_SEATS)))
        .addMethod(
          getWatchReservationStatusMethod(),
          io.grpc.stub.ServerCalls.asyncServerStreamingCall(
            new MethodHandlers<
              com.eventflow.inventory.v1.WatchReservationStatusRequest,
              com.eventflow.inventory.v1.ReservationStatusEvent>(
                service, METHODID_WATCH_RESERVATION_STATUS)))
        .addMethod(
          getCancelOperationMethod(),
          io.grpc.stub.ServerCalls.asyncUnaryCall(
            new MethodHandlers<
              com.eventflow.inventory.v1.CancelOperationRequest,
              com.eventflow.inventory.v1.CancelOperationResponse>(
                service, METHODID_CANCEL_OPERATION)))
        .build();
  }

  private static abstract class InventoryServiceBaseDescriptorSupplier
      implements io.grpc.protobuf.ProtoFileDescriptorSupplier, io.grpc.protobuf.ProtoServiceDescriptorSupplier {
    InventoryServiceBaseDescriptorSupplier() {}

    @java.lang.Override
    public com.google.protobuf.Descriptors.FileDescriptor getFileDescriptor() {
      return com.eventflow.inventory.v1.InventoryApiProto.getDescriptor();
    }

    @java.lang.Override
    public com.google.protobuf.Descriptors.ServiceDescriptor getServiceDescriptor() {
      return getFileDescriptor().findServiceByName("InventoryService");
    }
  }

  private static final class InventoryServiceFileDescriptorSupplier
      extends InventoryServiceBaseDescriptorSupplier {
    InventoryServiceFileDescriptorSupplier() {}
  }

  private static final class InventoryServiceMethodDescriptorSupplier
      extends InventoryServiceBaseDescriptorSupplier
      implements io.grpc.protobuf.ProtoMethodDescriptorSupplier {
    private final java.lang.String methodName;

    InventoryServiceMethodDescriptorSupplier(java.lang.String methodName) {
      this.methodName = methodName;
    }

    @java.lang.Override
    public com.google.protobuf.Descriptors.MethodDescriptor getMethodDescriptor() {
      return getServiceDescriptor().findMethodByName(methodName);
    }
  }

  private static volatile io.grpc.ServiceDescriptor serviceDescriptor;

  public static io.grpc.ServiceDescriptor getServiceDescriptor() {
    io.grpc.ServiceDescriptor result = serviceDescriptor;
    if (result == null) {
      synchronized (InventoryServiceGrpc.class) {
        result = serviceDescriptor;
        if (result == null) {
          serviceDescriptor = result = io.grpc.ServiceDescriptor.newBuilder(SERVICE_NAME)
              .setSchemaDescriptor(new InventoryServiceFileDescriptorSupplier())
              .addMethod(getCheckAvailabilityMethod())
              .addMethod(getReserveSeatsMethod())
              .addMethod(getWatchReservationStatusMethod())
              .addMethod(getCancelOperationMethod())
              .build();
        }
      }
    }
    return result;
  }
}
